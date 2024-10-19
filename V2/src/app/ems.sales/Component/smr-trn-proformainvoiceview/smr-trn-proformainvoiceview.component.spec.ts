import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SmrTrnProformainvoiceviewComponent } from './smr-trn-proformainvoiceview.component';

describe('SmrTrnProformainvoiceviewComponent', () => {
  let component: SmrTrnProformainvoiceviewComponent;
  let fixture: ComponentFixture<SmrTrnProformainvoiceviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SmrTrnProformainvoiceviewComponent]
    });
    fixture = TestBed.createComponent(SmrTrnProformainvoiceviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
