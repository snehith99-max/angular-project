import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceAdd360Component } from './smr-trn-invoice-add360.component';

describe('SmrTrnInvoiceAdd360Component', () => {
  let component: SmrTrnInvoiceAdd360Component;
  let fixture: ComponentFixture<SmrTrnInvoiceAdd360Component>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceAdd360Component]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceAdd360Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
