import { InMemoryDbService } from 'angular-in-memory-web-api';

export class InMemoryDataService implements InMemoryDbService {
  createDb() {
    const topQueries = [
      { id: 1, name: 'qwe asd' },
      { id: 2, name: 'wer sdf' },
      { id: 3, name: 'rty fgh' },
      { id: 4, name: 'tyu ghj' },
      { id: 5, name: 'yuihjk' },
      { id: 6, name: 'sasf' },
      { id: 7, name: 'xvdsg' },
      { id: 8, name: 'scvsvss' },
      { id: 9, name: 'xcvaas' },
      { id: 10, name: 'asdawd' },
      { id: 11, name: 'awdad' },
      { id: 12, name: 'asdwa' },
      { id: 13, name: 'awdsad' },
      { id: 14, name: 'asdwas' },
      { id: 15, name: 'sdsdf' }
    ];
    return {topQueries};
  }
}
