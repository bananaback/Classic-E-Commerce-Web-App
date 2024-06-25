/** @type {import('tailwindcss').Config} */
module.exports = {
  prefix: 'tw-',
  content: [
    './Areas/**/*.cshtml',
    './Views/**/*.cshtml',
    './Pages/**/*.cshtml',
    './wwwroot/js/**/*.js',
  ],
  theme: {
    extend: {
        width: {
            '128': '1280px', // 1280 pixels
        },
    },
  },
  plugins: [],
}

