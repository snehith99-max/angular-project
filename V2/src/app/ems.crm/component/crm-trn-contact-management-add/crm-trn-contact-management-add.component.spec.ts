import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactManagementAddComponent } from './crm-trn-contact-management-add.component';

describe('CrmTrnContactManagementAddComponent', () => {
  let component: CrmTrnContactManagementAddComponent;
  let fixture: ComponentFixture<CrmTrnContactManagementAddComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactManagementAddComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactManagementAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
