import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRaisesalesorder2invoiceComponent } from './smr-trn-raisesalesorder2invoice.component';

describe('SmrTrnRaisesalesorder2invoiceComponent', () => {
  let component: SmrTrnRaisesalesorder2invoiceComponent;
  let fixture: ComponentFixture<SmrTrnRaisesalesorder2invoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRaisesalesorder2invoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRaisesalesorder2invoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
