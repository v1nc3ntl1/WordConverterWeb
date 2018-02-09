import { TestBed, inject } from '@angular/core/testing';

import { WordconverterService } from './wordconverter.service';

describe('WordconverterService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WordconverterService]
    });
  });

  it('should be created', inject([WordconverterService], (service: WordconverterService) => {
    expect(service).toBeTruthy();
  }));
});
