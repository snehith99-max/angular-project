import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstConstitutionComponent } from './crm-mst-constitution.component';

describe('CrmMstConstitutionComponent', () => {
  let component: CrmMstConstitutionComponent;
  let fixture: ComponentFixture<CrmMstConstitutionComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstConstitutionComponent]
    });
    fixture = TestBed.createComponent(CrmMstConstitutionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
