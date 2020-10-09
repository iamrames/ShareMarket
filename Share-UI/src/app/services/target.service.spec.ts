/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { TargetService } from './target.service';

describe('Service: Target', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [TargetService]
    });
  });

  it('should ...', inject([TargetService], (service: TargetService) => {
    expect(service).toBeTruthy();
  }));
});
