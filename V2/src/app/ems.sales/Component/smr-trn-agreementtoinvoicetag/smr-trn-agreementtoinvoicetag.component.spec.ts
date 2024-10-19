import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnAgreementtoinvoicetagComponent } from './smr-trn-agreementtoinvoicetag.component';

describe('SmrTrnAgreementtoinvoicetagComponent', () => {
  let component: SmrTrnAgreementtoinvoicetagComponent;
  let fixture: ComponentFixture<SmrTrnAgreementtoinvoicetagComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnAgreementtoinvoicetagComponent]
    });
    fixture = TestBed.createComponent(SmrTrnAgreementtoinvoicetagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
