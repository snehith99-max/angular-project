import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnReceiptapprovalComponent } from './smr-trn-receiptapproval.component';

describe('SmrTrnReceiptapprovalComponent', () => {
  let component: SmrTrnReceiptapprovalComponent;
  let fixture: ComponentFixture<SmrTrnReceiptapprovalComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnReceiptapprovalComponent]
    });
    fixture = TestBed.createComponent(SmrTrnReceiptapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
