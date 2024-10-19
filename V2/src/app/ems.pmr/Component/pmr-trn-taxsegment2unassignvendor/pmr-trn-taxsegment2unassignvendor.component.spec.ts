import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnTaxsegment2unassignvendorComponent } from './pmr-trn-taxsegment2unassignvendor.component';

describe('PmrTrnTaxsegment2unassignvendorComponent', () => {
  let component: PmrTrnTaxsegment2unassignvendorComponent;
  let fixture: ComponentFixture<PmrTrnTaxsegment2unassignvendorComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnTaxsegment2unassignvendorComponent]
    });
    fixture = TestBed.createComponent(PmrTrnTaxsegment2unassignvendorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
