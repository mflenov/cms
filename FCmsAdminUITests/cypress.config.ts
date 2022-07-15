import { defineConfig } from 'cypress'

export default defineConfig({
  e2e: {
    supportFile: false,
    baseUrl: 'http://localhost:4200/',
    setupNodeEvents(on, config) {},
    specPattern: 'cypress/tests/**/*.cy.{js,jsx,ts,tsx}',
  },
})
