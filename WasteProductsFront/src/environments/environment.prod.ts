export const environment = {
    production: true,
    logLevel: 3,

    /* Api */
    apiHostUrl: 'https://localhost:44362',
    // apiHostUrl: 'http://waste-api.belpyro.net',

    /* Identity */
    iderntityHostUrl: this.apiHostUrl + '/identity',
    clientId: 'wasteproducts.front.angular',

    scope: 'openid profile email wasteproducts-api',
    dummyClientSecret: 'F0E56438-BCDE-401E-BDE5-303BA812186F',
};
