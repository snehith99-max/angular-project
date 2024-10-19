import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RblTrnDeliverytoinvoiceComponent } from './rbl-trn-deliverytoinvoice.component';

describe('RblTrnDeliverytoinvoiceComponent', () => {
  let component: RblTrnDeliverytoinvoiceComponent;
  let fixture: ComponentFixture<RblTrnDeliverytoinvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RblTrnDeliverytoinvoiceComponent]
    });
    fixture = TestBed.createComponent(RblTrnDeliverytoinvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
