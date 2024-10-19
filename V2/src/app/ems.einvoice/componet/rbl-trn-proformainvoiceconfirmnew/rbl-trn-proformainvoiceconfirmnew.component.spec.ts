import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RblTrnProformainvoiceconfirmnewComponent } from './rbl-trn-proformainvoiceconfirmnew.component';

describe('RblTrnProformainvoiceconfirmnewComponent', () => {
  let component: RblTrnProformainvoiceconfirmnewComponent;
  let fixture: ComponentFixture<RblTrnProformainvoiceconfirmnewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RblTrnProformainvoiceconfirmnewComponent]
    });
    fixture = TestBed.createComponent(RblTrnProformainvoiceconfirmnewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
