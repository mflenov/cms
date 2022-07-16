describe("Content", () => {
    beforeEach(() => {
        cy.intercept('GET', 'http://localhost:56710/api/v1/config/filters', { fixture: 'list-filters.json' }).as('list-filters')
    });

    beforeEach(() => {
        cy.intercept('GET', 'http://localhost:56710/api/v1/data/enums', { fixture: 'data-enums.json' }).as('data-enums')
    });

    it ('navigate to the home page', () => {
        // home page
        cy.visit('/');
        expect(cy.get('div.page')).to.exist;

        // go to the config page
        cy.get('.header .nav-link.config-link').click();        
        expect(cy.get('h1').contains('Config')).to.exist;

        // go to the filters page
        cy.get('a[data-cy=filterslist-link]').click();
        expect(cy.get('h1').contains('Filters')).to.exist;
        cy.wait('@list-filters', {timeout: 1000});

        // add filter
        cy.get('button[data-cy=add-button]').click();
        expect(cy.get('h1').contains('Edit filter')).to.exist;
        cy.wait('@data-enums', {timeout: 1000});
    });
});