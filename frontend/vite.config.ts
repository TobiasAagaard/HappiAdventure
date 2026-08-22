import { defineConfig } from 'vite'
import react, { reactCompilerPreset } from '@vitejs/plugin-react'
import babel from '@rolldown/plugin-babel'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    babel({ presets: [reactCompilerPreset()] })
  ],
  // maplibre-gl ships its web worker as a separate entry the dep optimizer
  // can't resolve; let it be served unbundled instead.
  optimizeDeps: {
    exclude: ['maplibre-gl'],
  },
})
