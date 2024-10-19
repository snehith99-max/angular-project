import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PmrTrnAgreementtoinvoicetagComponent } from './pmr-trn-agreementtoinvoicetag.component';

describe('PmrTrnAgreementtoinvoicetagComponent', () => {
  let component: PmrTrnAgreementtoinvoicetagComponent;
  let fixture: ComponentFixture<PmrTrnAgreementtoinvoicetagComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PmrTrnAgreementtoinvoicetagComponent]
    });
    fixture = TestBed.createComponent(PmrTrnAgreementtoinvoicetagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
