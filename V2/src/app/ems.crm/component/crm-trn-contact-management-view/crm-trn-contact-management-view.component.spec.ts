import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactManagementViewComponent } from './crm-trn-contact-management-view.component';

describe('CrmTrnContactManagementViewComponent', () => {
  let component: CrmTrnContactManagementViewComponent;
  let fixture: ComponentFixture<CrmTrnContactManagementViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactManagementViewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactManagementViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
