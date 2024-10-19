import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceeditComponent } from './smr-trn-invoiceedit.component';

describe('SmrTrnInvoiceeditComponent', () => {
  let component: SmrTrnInvoiceeditComponent;
  let fixture: ComponentFixture<SmrTrnInvoiceeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceeditComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
