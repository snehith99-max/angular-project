import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnRaiseorder2ProformainvoiceComponent } from './smr-trn-raiseorder2-proformainvoice.component';

describe('SmrTrnRaiseorder2ProformainvoiceComponent', () => {
  let component: SmrTrnRaiseorder2ProformainvoiceComponent;
  let fixture: ComponentFixture<SmrTrnRaiseorder2ProformainvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnRaiseorder2ProformainvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnRaiseorder2ProformainvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
