import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PblTrnDirectinvoiceAddComponent } from './pbl-trn-directinvoice-add.component';

describe('PblTrnDirectinvoiceAddComponent', () => {
  let component: PblTrnDirectinvoiceAddComponent;
  let fixture: ComponentFixture<PblTrnDirectinvoiceAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [PblTrnDirectinvoiceAddComponent]
    });
    fixture = TestBed.createComponent(PblTrnDirectinvoiceAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
