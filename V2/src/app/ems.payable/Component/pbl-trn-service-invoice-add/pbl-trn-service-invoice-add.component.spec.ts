import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnServiceInvoiceAddComponent } from './pbl-trn-service-invoice-add.component';

describe('PblTrnServiceInvoiceAddComponent', () => {
  let component: PblTrnServiceInvoiceAddComponent;
  let fixture: ComponentFixture<PblTrnServiceInvoiceAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnServiceInvoiceAddComponent]
    });
    fixture = TestBed.createComponent(PblTrnServiceInvoiceAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
