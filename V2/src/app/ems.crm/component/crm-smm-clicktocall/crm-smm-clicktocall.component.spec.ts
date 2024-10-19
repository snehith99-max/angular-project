import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmClicktocallComponent } from './crm-smm-clicktocall.component';

describe('CrmSmmClicktocallComponent', () => {
  let component: CrmSmmClicktocallComponent;
  let fixture: ComponentFixture<CrmSmmClicktocallComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmClicktocallComponent]
    });
    fixture = TestBed.createComponent(CrmSmmClicktocallComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
