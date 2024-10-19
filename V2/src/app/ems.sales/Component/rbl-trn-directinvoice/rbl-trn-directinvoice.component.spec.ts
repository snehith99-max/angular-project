import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RblTrnDirectinvoiceComponent } from './rbl-trn-directinvoice.component';

describe('RblTrnDirectinvoiceComponent', () => {
  let component: RblTrnDirectinvoiceComponent;
  let fixture: ComponentFixture<RblTrnDirectinvoiceComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RblTrnDirectinvoiceComponent]
    });
    fixture = TestBed.createComponent(RblTrnDirectinvoiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
