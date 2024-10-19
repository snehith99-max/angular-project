import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactManagementIndividualEditComponent } from './crm-trn-contact-management-individual-edit.component';

describe('CrmTrnContactManagementIndividualEditComponent', () => {
  let component: CrmTrnContactManagementIndividualEditComponent;
  let fixture: ComponentFixture<CrmTrnContactManagementIndividualEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactManagementIndividualEditComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactManagementIndividualEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
