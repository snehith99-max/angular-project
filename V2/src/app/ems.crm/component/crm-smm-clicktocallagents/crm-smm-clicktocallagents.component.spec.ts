import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmClicktocallagentsComponent } from './crm-smm-clicktocallagents.component';

describe('CrmSmmClicktocallagentsComponent', () => {
  let component: CrmSmmClicktocallagentsComponent;
  let fixture: ComponentFixture<CrmSmmClicktocallagentsComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmClicktocallagentsComponent]
    });
    fixture = TestBed.createComponent(CrmSmmClicktocallagentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
