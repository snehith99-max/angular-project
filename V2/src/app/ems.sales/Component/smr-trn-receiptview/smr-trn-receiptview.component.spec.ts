import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnReceiptviewComponent } from './smr-trn-receiptview.component';

describe('SmrTrnReceiptviewComponent', () => {
  let component: SmrTrnReceiptviewComponent;
  let fixture: ComponentFixture<SmrTrnReceiptviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnReceiptviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnReceiptviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
