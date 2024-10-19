import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnRatecontractComponent } from './pmr-trn-ratecontract.component';

describe('PmrTrnRatecontractComponent', () => {
  let component: PmrTrnRatecontractComponent;
  let fixture: ComponentFixture<PmrTrnRatecontractComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnRatecontractComponent]
    });
    fixture = TestBed.createComponent(PmrTrnRatecontractComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
