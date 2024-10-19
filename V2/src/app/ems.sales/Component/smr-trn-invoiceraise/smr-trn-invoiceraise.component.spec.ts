import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnInvoiceraiseComponent } from './smr-trn-invoiceraise.component';

describe('SmrTrnInvoiceraiseComponent', () => {
  let component: SmrTrnInvoiceraiseComponent;
  let fixture: ComponentFixture<SmrTrnInvoiceraiseComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnInvoiceraiseComponent]
    });
    fixture = TestBed.createComponent(SmrTrnInvoiceraiseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
