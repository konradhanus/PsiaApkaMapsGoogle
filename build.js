const { execSync } = require('child_process');
const path = require('path');
const fs = require('fs');

// Ścieżki do projektu Unity i pliku Xcode
const unityProjectPath = '/Users/konradhanus/Desktop/GIT/PsiaApkaMapsGoogle';
const xcodeProjectPath = '/Users/konradhanus/Desktop/GIT/_builds2';
const xcodeArchivePath = '/Users/konradhanus/Desktop/GIT/_builds/archive';

// Funkcja do budowania projektu Unity dla iOS
function buildUnityProject() {
    try {
        console.log('Building Unity project...');
        const unityCommand = `/Applications/Unity/Hub/Editor/2022.3.18f1/Unity.app/Contents/MacOS/Unity -quit -batchmode -projectPath "${unityProjectPath}" -executeMethod BuildScript.iOS -logFile build.log`;
        execSync(unityCommand, { stdio: 'inherit' });
        console.log('Unity build completed.');
    } catch (error) {
        console.error('Failed to build Unity project:', error);
        process.exit(1);
    }
}

// Funkcja do instalacji zależności CocoaPods
function installPods() {
    try {
        console.log('Installing CocoaPods dependencies...');
        const podPath = '/usr/local/bin/pod';
        execSync(`${podPath} install`, { cwd: xcodeProjectPath, stdio: 'inherit', shell: '/bin/bash' });
        execSync(`${podPath} update`, { cwd: xcodeProjectPath, stdio: 'inherit', shell: '/bin/bash' });
        console.log('CocoaPods installation completed.');
    } catch (error) {
        console.error('Failed to install CocoaPods dependencies:', error);
        process.exit(1);
    }
}

// Funkcja do ustawienia zespołu deweloperskiego w pliku project.pbxproj
function setDevelopmentTeam() {
    const projectFilePath = path.join(xcodeProjectPath, 'Unity-iPhone.xcodeproj', 'project.pbxproj');
    const teamID = '683MXZZR23'; // Wstaw swój Team ID tutaj

    let projectFileContent = fs.readFileSync(projectFilePath, 'utf8');

    // Regularne wyrażenie do znalezienia sekcji build configuration
    const regex = /(buildSettings = {[\s\S]*?};)/g;
    const matches = projectFileContent.match(regex);

    if (matches) {
        matches.forEach(match => {
            // Dodanie/aktualizacja zespołu deweloperskiego
            if (!match.includes('DEVELOPMENT_TEAM')) {
                const updatedMatch = match.replace(
                    '}',
                    `DEVELOPMENT_TEAM = "${teamID}";\n}`
                );
                projectFileContent = projectFileContent.replace(match, updatedMatch);
            }
        });
    } else {
        console.error('Failed to find build settings section in project file.');
        process.exit(1);
    }

    // Zapisz zmieniony plik
    fs.writeFileSync(projectFilePath, projectFileContent);
    console.log('Development team set successfully.');
}

// Funkcja do archiwizacji projektu w Xcode
function archiveXcodeProject() {
    try {
        console.log('Archiving Xcode project...');
        const archiveCommand = `xcodebuild -project "${path.join(xcodeProjectPath, 'Unity-iPhone.xcodeproj')}" -scheme Unity-iPhone archive -archivePath "${xcodeArchivePath}" -allowProvisioningUpdates`;
        execSync(archiveCommand, { stdio: 'inherit' });
        console.log('Xcode archive completed.');
    } catch (error) {
        console.error('Failed to archive Xcode project:', error);
        process.exit(1);
    }
}

// Główna funkcja uruchamiająca cały proces
function main() {
    buildUnityProject();
    installPods(); // Dodany krok instalacji CocoaPods
    setDevelopmentTeam(); // Dodany krok ustawienia zespołu deweloperskiego
    archiveXcodeProject();
}

main();
