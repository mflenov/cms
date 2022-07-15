describe("Content", () => {
    it ('navigate to the home page', () => {
        cy.visit('/');

        expect(cy.get('div.page')).to.exist;
    });
});