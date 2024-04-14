const fs = require('fs');
const https = require('https');

function downloadFile(url, fileName) {
    return new Promise((resolve, reject) => {
        const file = fs.createWriteStream(fileName);

        https.get(url, response => {
            response.pipe(file);

            file.on('finish', () => {
                file.close(resolve);
            });
        }).on('error', error => {
            fs.unlink(fileName);
            reject(error.message);
        });
    });
}

const files = [
    { url: 'https://psiaapka.pl/project/FirebaseCppApp-11_8_1.bundle', fileName: 'FirebaseCppApp-11_8_1.bundle' },
    { url: 'https://psiaapka.pl/project/FirebaseCppApp-11_8_1.so', fileName: 'FirebaseCppApp-11_8_1.so' }
];

async function downloadFiles() {
    for (const file of files) {
        try {
            await downloadFile(file.url, file.fileName);
            console.log(`Plik ${file.fileName} został pobrany.`);
        } catch (error) {
            console.error(`Wystąpił problem podczas pobierania pliku ${file.fileName}: ${error}`);
        }
    }
}

downloadFiles();
