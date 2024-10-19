import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnTaxsegment2assignvendorComponent } from './pmr-trn-taxsegment2assignvendor.component';

describe('PmrTrnTaxsegment2assignvendorComponent', () => {
  let component: PmrTrnTaxsegment2assignvendorComponent;
  let fixture: ComponentFixture<PmrTrnTaxsegment2assignvendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnTaxsegment2assignvendorComponent]
    });
    fixture = TestBed.createComponent(PmrTrnTaxsegment2assignvendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
