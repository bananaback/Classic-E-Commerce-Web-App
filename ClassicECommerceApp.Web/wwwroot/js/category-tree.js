let categories = [];

$(document).ready(function () {
    // Fetch data from the API endpoint
    $.ajax({
        url: 'https://localhost:7266/api/shop/productcategories',
        method: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data && data.length > 0) {
                categories = buildTree(data);
                renderRootCategories();
            } else {
                console.log("empty data", data);
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching categories:', error);
        }
    });
});

function buildTree(categories) {
    let tree = [];
    let categoryMap = {};

    // Create a map to reference categories by their id
    categories.forEach(function (category) {
        category.children = [];
        categoryMap[category.id] = category;
    });

    // Populate the tree based on parent-child relationships
    categories.forEach(function (category) {
        if (category.parentId !== null) {
            let parentCategory = categoryMap[category.parentId];
            if (parentCategory) {
                parentCategory.children.push(category);
            }
        } else {
            tree.push(category); // Root level category
        }
    });

    return tree;
}

function renderRootCategories() {
    const $rootCategoriesUL = $('.root-categories').children('ul').first();

    function createDropdownContent(categories, depth = 0) {
        const leftClass = depth === 0 ? 'tw-left-0' : 'tw-left-full';
        const topClass = depth === 0 ? 'tw-top-12' : 'tw-top-0';
        const $dropdownContent = $('<ul></ul>')
            .addClass(`tw-w-36 tw-flex tw-flex-col tw-bg-white tw-border-2 tw-border-gray-200 tw-rounded-lg tw-absolute drop-down ${leftClass} ${topClass}`)
            .hide();

        categories.forEach(category => {
            const $categoryLI = $('<li></li>')
                .addClass('tw-relative tw-cursor-pointer hover:tw-bg-blue-400 tw-w-full tw-px-5 tw-py-1 tw-rounded-lg')
                .append($('<div></div>')
                    .addClass('tw-text-black')
                    .text(category.name));

            const $arrowDiv = $('<div></div>')
                .addClass('tw-absolute tw-right-0 tw-top-1/2 -tw-translate-y-1/2 tw-font-semibold')
                .append('<svg xmlns="http://www.w3.org/2000/svg" height="24px" viewBox="0 -960 960 960" width="24px" fill="#efefef"><path d="M504-480 320-664l56-56 240 240-240 240-56-56 184-184Z"/></svg>');

            if (category.children && category.children.length > 0) {
                $categoryLI.append($arrowDiv);
                const $childDropdownContent = createDropdownContent(category.children, depth + 1);
                $categoryLI.append($childDropdownContent);
                $categoryLI.hover(
                    function () {
                        // Mouse enter
                        $childDropdownContent.show();
                    },
                    function () {
                        // Mouse leave
                        $childDropdownContent.hide();
                    }
                );
            }

            $dropdownContent.append($categoryLI);
        });

        return $dropdownContent;
    }

    categories.forEach(category => {
        const $categoryLI = $('<li></li>')
            .addClass('tw-z-10 tw-relative tw-cursor-pointer tw-flex tw-items-center tw-px-4 hover:tw-bg-blue-400 tw-h-full tw-text-center')
            .text(category.name);

        if (category.children && category.children.length > 0) {
            const $dropdownContent = createDropdownContent(category.children);
            $categoryLI.append($dropdownContent);
            $categoryLI.hover(
                function () {
                    // Mouse enter
                    $dropdownContent.show();
                },
                function () {
                    // Mouse leave
                    $dropdownContent.hide();
                }
            );
        }

        $rootCategoriesUL.append($categoryLI);
    });
}
