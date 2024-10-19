import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTcontactmanagementComponent } from './crm-trn-tcontactmanagement.component';

describe('CrmTrnTcontactmanagementComponent', () => {
  let component: CrmTrnTcontactmanagementComponent;
  let fixture: ComponentFixture<CrmTrnTcontactmanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTcontactmanagementComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTcontactmanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
