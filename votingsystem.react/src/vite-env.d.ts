/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_APP_API_BASEURL: string;
}
  
interface ImportMeta {
    readonly env: ImportMetaEnv
}