import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmMstSalutationsComponent } from './crm-mst-salutations.component';

describe('CrmMstSalutationsComponent', () => {
  let component: CrmMstSalutationsComponent;
  let fixture: ComponentFixture<CrmMstSalutationsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmMstSalutationsComponent]
    });
    fixture = TestBed.createComponent(CrmMstSalutationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
