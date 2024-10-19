import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnInvoiceApprovalAddComponent } from './pbl-trn-invoice-approval-add.component';

describe('PblTrnInvoiceApprovalAddComponent', () => {
  let component: PblTrnInvoiceApprovalAddComponent;
  let fixture: ComponentFixture<PblTrnInvoiceApprovalAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnInvoiceApprovalAddComponent]
    });
    fixture = TestBed.createComponent(PblTrnInvoiceApprovalAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
