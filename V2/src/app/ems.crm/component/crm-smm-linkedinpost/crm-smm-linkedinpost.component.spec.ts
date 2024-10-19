import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmLinkedinpostComponent } from './crm-smm-linkedinpost.component';

describe('CrmSmmLinkedinpostComponent', () => {
  let component: CrmSmmLinkedinpostComponent;
  let fixture: ComponentFixture<CrmSmmLinkedinpostComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmLinkedinpostComponent]
    });
    fixture = TestBed.createComponent(CrmSmmLinkedinpostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
