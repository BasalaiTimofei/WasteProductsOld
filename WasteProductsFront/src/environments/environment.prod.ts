export const environment = {
  production: true,
  logLevel: 3,

  /* Api */
     apiHostUrl: 'http://localhost:2189',
   //apiHostUrl: 'http://waste-api.belpyro.net',

  /* Identity */
  // iderntityHostUrl: 'https://localhost:44362/identity',
  //iderntityHostUrl: 'https://localhost:44333/identity',
  iderntityHostUrl: 'https://localhost:44378/identity',
  clientId: 'wasteproducts.front.angular',

  scope : 'openid profile email wasteproducts-api',
  dummyClientSecret: 'F0E56438-BCDE-401E-BDE5-303BA812186F',

  urlDiscoveryDocument : 'https://localhost:44378/identity/.well-known/openid-configuration'
  //urlDiscoveryDocument : 'https://waste-api.belpyro.net/identity/.well-known/openid-configuration'
 


};
