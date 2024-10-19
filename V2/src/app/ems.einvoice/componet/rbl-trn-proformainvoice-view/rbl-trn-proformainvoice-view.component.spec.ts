import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RblTrnProformainvoiceViewComponent } from './rbl-trn-proformainvoice-view.component';

describe('RblTrnProformainvoiceViewComponent', () => {
  let component: RblTrnProformainvoiceViewComponent;
  let fixture: ComponentFixture<RblTrnProformainvoiceViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RblTrnProformainvoiceViewComponent]
    });
    fixture = TestBed.createComponent(RblTrnProformainvoiceViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
