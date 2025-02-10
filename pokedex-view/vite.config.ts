import {defineConfig} from 'vite'
import {readFileSync} from 'fs';
import {resolve} from 'path';
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    define: {
        __APP_VERSION__: JSON.stringify(getAppVersion()),
        __BUILD_ID__: Date.now().toString(),
    },
    resolve: {
        alias: {
            '@pages': '/src/pages',
            '@components': '/src/components',
            '@layouts': '/src/layouts',
            '@shared': '/src/shared'
        }
    }
})

// Function to get the app version from package.json
function getAppVersion(): string {
    const packageJson = readFileSync(resolve(".", 'package.json'), 'utf-8');
    const {version} = JSON.parse(packageJson);
    return version;
}

