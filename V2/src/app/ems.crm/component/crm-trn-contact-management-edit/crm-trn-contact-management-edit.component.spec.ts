import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactManagementEditComponent } from './crm-trn-contact-management-edit.component';

describe('CrmTrnContactManagementEditComponent', () => {
  let component: CrmTrnContactManagementEditComponent;
  let fixture: ComponentFixture<CrmTrnContactManagementEditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactManagementEditComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactManagementEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
