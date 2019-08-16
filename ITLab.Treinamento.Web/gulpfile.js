var fs = require('fs');
var gulp = require('gulp');
var i18nGenerator = require('i18n-generator');
var browserSync = require('browser-sync').create();

var createDir = function (dest) {
    return new Promise((resolve, reject) => {
        console.log(`Creating folder: ${dest}`);
        fs.mkdir(dest, err => {
            if (err) reject(err);

            resolve();
        });
    });
}

var writeFile = function (dest, content) {
    return new Promise((resolve, reject) => {
        console.log(`Writing: ${dest}`);
        fs.writeFile(dest, content, (err) => {
            if (err) reject(err);

            resolve();
        });
    });
}

var translate = function (file) {
    if (file.path) file = file.path.replace(/\\/g, '/');

    return new Promise((resolve, reject) => {
        i18nGenerator.get(`${file}`, (err, data) => {
            if (err) reject(err);

            var filename = file.substring(file.lastIndexOf('/') + 1, file.lastIndexOf('.'));
            console.log(`Translating: ${file}`);

            var promises = [];

            var localesFolder = 'public/locales';
            if (!fs.existsSync(localesFolder))
                promises.push(createDir(localesFolder));

            for (var language in data) {
                var languageFolder = `${localesFolder}/${language}`;

                if (!fs.existsSync(languageFolder))
                    promises.push(createDir(languageFolder));

                var dest = `${languageFolder}/${filename}.json`;
                var content = JSON.stringify(data[language]);
                
                promises.push(writeFile(dest, content));
            }

            Promise.all(promises)
                .then(() => resolve())
                .catch(e => reject(e));
        });
    });
}

gulp.task('translate', () => {
    return new Promise((resolve, reject) => {
        fs.readdir('assets/locales', (err, files) => {
            if (err) throw err;

            var promises = [];
            for (var i = 0; i < files.length; i++) {
                if (files[i].indexOf('.txt') > 0)
                    promises.push(translate(`assets/locales/${files[i]}`));
            }

            Promise.all(promises).then(resolve).catch(reject);
        });
    });
});

gulp.task('serve', ['translate'], () => {
    browserSync.init({ server: "./", port: 8080 });

    gulp.watch('assets/locales/*.txt').on('change', translate);
    gulp.watch('app/**/*.{html,js}').on('change', browserSync.reload);
    gulp.watch('public/**/*.{css,json}').on('change', browserSync.reload);
});

gulp.task('default', ['serve']);
