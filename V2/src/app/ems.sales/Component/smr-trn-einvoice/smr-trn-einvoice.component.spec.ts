import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnEinvoiceComponent } from './smr-trn-einvoice.component';

describe('SmrTrnEinvoiceComponent', () => {
  let component: SmrTrnEinvoiceComponent;
  let fixture: ComponentFixture<SmrTrnEinvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnEinvoiceComponent]
    });
    fixture = TestBed.createComponent(SmrTrnEinvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
