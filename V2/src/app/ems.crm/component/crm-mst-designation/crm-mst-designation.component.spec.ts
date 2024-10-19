import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstDesignationComponent } from './crm-mst-designation.component';

describe('CrmMstDesignationComponent', () => {
  let component: CrmMstDesignationComponent;
  let fixture: ComponentFixture<CrmMstDesignationComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstDesignationComponent]
    });
    fixture = TestBed.createComponent(CrmMstDesignationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
