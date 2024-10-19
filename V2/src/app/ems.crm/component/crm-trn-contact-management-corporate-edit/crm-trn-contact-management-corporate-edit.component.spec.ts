import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactManagementCorporateEditComponent } from './crm-trn-contact-management-corporate-edit.component';

describe('CrmTrnContactManagementCorporateEditComponent', () => {
  let component: CrmTrnContactManagementCorporateEditComponent;
  let fixture: ComponentFixture<CrmTrnContactManagementCorporateEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactManagementCorporateEditComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactManagementCorporateEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
