import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTfilemanagementComponent } from './crm-trn-tfilemanagement.component';

describe('CrmTrnTfilemanagementComponent', () => {
  let component: CrmTrnTfilemanagementComponent;
  let fixture: ComponentFixture<CrmTrnTfilemanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTfilemanagementComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTfilemanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
