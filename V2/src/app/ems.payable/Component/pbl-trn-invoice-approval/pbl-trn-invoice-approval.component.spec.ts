import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnInvoiceApprovalComponent } from './pbl-trn-invoice-approval.component';

describe('PblTrnInvoiceApprovalComponent', () => {
  let component: PblTrnInvoiceApprovalComponent;
  let fixture: ComponentFixture<PblTrnInvoiceApprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnInvoiceApprovalComponent]
    });
    fixture = TestBed.createComponent(PblTrnInvoiceApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
