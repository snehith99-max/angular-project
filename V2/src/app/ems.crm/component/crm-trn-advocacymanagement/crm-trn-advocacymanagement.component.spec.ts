import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnAdvocacymanagementComponent } from './crm-trn-advocacymanagement.component';

describe('CrmTrnAdvocacymanagementComponent', () => {
  let component: CrmTrnAdvocacymanagementComponent;
  let fixture: ComponentFixture<CrmTrnAdvocacymanagementComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnAdvocacymanagementComponent]
    });
    fixture = TestBed.createComponent(CrmTrnAdvocacymanagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
