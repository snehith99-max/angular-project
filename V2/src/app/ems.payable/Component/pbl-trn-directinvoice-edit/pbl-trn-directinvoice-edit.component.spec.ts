import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnDirectinvoiceEditComponent } from './pbl-trn-directinvoice-edit.component';

describe('PblTrnDirectinvoiceEditComponent', () => {
  let component: PblTrnDirectinvoiceEditComponent;
  let fixture: ComponentFixture<PblTrnDirectinvoiceEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnDirectinvoiceEditComponent]
    });
    fixture = TestBed.createComponent(PblTrnDirectinvoiceEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
