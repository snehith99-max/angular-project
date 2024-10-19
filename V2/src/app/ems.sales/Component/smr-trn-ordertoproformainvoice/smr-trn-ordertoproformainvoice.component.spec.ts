import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnOrdertoproformainvoiceComponent } from './smr-trn-ordertoproformainvoice.component';

describe('SmrTrnOrdertoproformainvoiceComponent', () => {
  let component: SmrTrnOrdertoproformainvoiceComponent;
  let fixture: ComponentFixture<SmrTrnOrdertoproformainvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnOrdertoproformainvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnOrdertoproformainvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
